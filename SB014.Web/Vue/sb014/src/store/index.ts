import { AxiosResponse } from "axios";
import { createStore } from "vuex";
import { ITournamentModel } from "@/models/ITournamentModel";
import { ISubscriberModel } from "@/models/ISubscriberModel";
import { SubscribeToTournamentModel } from "@/models/SubscribeToTournamentModel";
import { TournamentService } from "@/services/TournamentService";
import { IGameModel } from "@/models/IGameModel";
import { IClueModel } from "@/models/IClueModel";

// State
interface AppState {
  currentSubscriber: ISubscriberModel | null;
  currentTournament: ITournamentModel | null;
  tournaments: ITournamentModel[];
  currentInPlayGame: IGameModel | null;
  cluesAnswered: IClueModel[] | null;
}
const state: AppState = {
  tournaments: [],
  currentTournament: null,
  currentSubscriber: null,
  currentInPlayGame: null,
  cluesAnswered: null,
};

// Mutautions and Actions
export enum MutationTypes {
  SET_CLUES_ANSWERED = "SET_CLUES_ANSWERED",
  SET_CURRENT_INPLAY_GAME = "SET_INPLAY_GAME",
  SET_CURRENT_SUBSCRIBER = "SET_CURRENT_SUBSCRIBER",
  SET_CURRENT_TOURNAMENT = "SET_CURRENT_TOURNAMENT",
  SET_TOURNAMENTS = "SET_TOURNAMENTS",
}

export enum ActionTypes {
  LOAD_CURRENT_TOURNAMENT = "LOAD_CURRENT_TOURNAMENT",
  LOAD_CURRENT_INPLAY_GAME = "LOAD_INPLAY_GAME",
  LOAD_TOURNAMENTS = "LOAD_TOURNAMENTS",
  UNSUBSCRIBE_FROM_TOURNAMENT = "UNSUBSCRIBE_FROM_TOURNAMENT",
  SUBMIT_ANSWER = "SUBMIT_ANSWER",
  SUBSCRIBE_TO_TOURNAMENT = "SUBSCRIBE_TO_TOURNAMENT",
}

export default createStore({
  state,
  getters: {
    cluesAnswered(state): IClueModel[] {
      return state.cluesAnswered || [];
    },
    currentSubscriber(state): ISubscriberModel | null {
      return state.currentSubscriber;
    },
    currentTournament(state): ITournamentModel | null {
      return state.currentTournament;
    },
    currentInPlayGame(state): IGameModel | null {
      return state.currentInPlayGame;
    },
    tournaments(state): ITournamentModel[] {
      return state.tournaments;
    },
  },
  mutations: {
    [MutationTypes.SET_CLUES_ANSWERED](state, payload: IClueModel[]) {
      state.cluesAnswered = payload;
    },
    [MutationTypes.SET_CURRENT_INPLAY_GAME](state, payload: IGameModel) {
      state.currentInPlayGame = payload;
    },
    [MutationTypes.SET_CURRENT_SUBSCRIBER](state, payload: ISubscriberModel) {
      state.currentSubscriber = payload;
    },
    [MutationTypes.SET_CURRENT_TOURNAMENT](state, payload: ITournamentModel) {
      state.currentTournament = payload;
    },
    [MutationTypes.SET_TOURNAMENTS](state, payload: ITournamentModel[]) {
      state.tournaments = payload;
    },
  },
  actions: {
    /**
     * Load inplay game of current tournament
     * @param context
     */
    async [ActionTypes.LOAD_CURRENT_INPLAY_GAME](context) {
      const service: TournamentService = new TournamentService();
      const tournamentId: string | undefined =
        context.state.currentSubscriber?.tournamentId;
      const inPlayGameId: string | undefined =
        context.state.currentTournament?.inplayGameId;
      if (tournamentId && inPlayGameId) {
        await service
          .getGame(tournamentId, inPlayGameId)
          .then((response: AxiosResponse<IGameModel>) => {
            context.commit(
              MutationTypes.SET_CURRENT_INPLAY_GAME,
              response.data
            );
            context.commit(MutationTypes.SET_CLUES_ANSWERED, null);
          });
      }
    },
    /**
     * Load tournament currently subscribed to and set as the current tournament
     * @param context
     */
    async [ActionTypes.LOAD_CURRENT_TOURNAMENT](context) {
      const service: TournamentService = new TournamentService();
      const tournamentId: string =
        context.state.currentSubscriber?.tournamentId || "";
      await service
        .getTournament(tournamentId)
        .then((response: AxiosResponse<ITournamentModel>) => {
          context.commit(MutationTypes.SET_CURRENT_TOURNAMENT, response.data);
        });
    },
    /**
     * Load all tournaments
     * @param context
     */
    async [ActionTypes.LOAD_TOURNAMENTS](context) {
      const service: TournamentService = new TournamentService();
      await service
        .getTournaments()
        .then((response: AxiosResponse<ITournamentModel[]>) => {
          context.commit(MutationTypes.SET_TOURNAMENTS, response.data);
        });
    },
    /**
     * Unsubscribe current subscribver from current tournament
     * @param context
     */
    async [ActionTypes.UNSUBSCRIBE_FROM_TOURNAMENT](context) {
      const service: TournamentService = new TournamentService();
      const subscriber: ISubscriberModel | null =
        context.state.currentSubscriber;
      if (
        context.state.currentTournament &&
        subscriber?.tournamentId &&
        subscriber?.id
      ) {
        await service
          .unsubscribeFromTournament(subscriber.tournamentId, subscriber.id)
          .then(() => {
            context.commit(MutationTypes.SET_CURRENT_INPLAY_GAME, null);
            context.commit(MutationTypes.SET_CURRENT_SUBSCRIBER, null);
            context.commit(MutationTypes.SET_CURRENT_TOURNAMENT, null);
            context.commit(MutationTypes.SET_CLUES_ANSWERED, null);
          });
      }
    },
    /**
     * Subscribe to a tournament
     * @param context
     * @param payload
     */
    async [ActionTypes.SUBSCRIBE_TO_TOURNAMENT](
      context,
      props: { id: string; payload: SubscribeToTournamentModel }
    ) {
      const service: TournamentService = new TournamentService();
      await service
        .subscribeToTournament(props.id, props.payload)
        .then((response: AxiosResponse<ISubscriberModel>) => {
          context.commit(MutationTypes.SET_CURRENT_SUBSCRIBER, response.data);
        });
    },
    /**
     * Submit an answer
     * @param context
     * @param payload
     */
    async [ActionTypes.SUBMIT_ANSWER](
      context,
      props: { clue: IClueModel; payload: string }
    ): Promise<boolean> {
      let correct = false;
      const subscriber: ISubscriberModel | null =
        context.state.currentSubscriber;
      if (
        context.state.currentTournament &&
        subscriber?.tournamentId &&
        subscriber?.id &&
        context.state.currentInPlayGame
      ) {
        const service: TournamentService = new TournamentService();
        await service
          .submitAnswer(
            subscriber.tournamentId,
            subscriber.id,
            context.state.currentInPlayGame.id,
            props.clue.id,
            props.payload
          )
          .then((response: AxiosResponse) => {
            correct = response.status == 200; //OK response means correct answer
          })
          .catch(() => {
            correct = false;
          })
          .finally(() => {
            // Record the answer
            const cluesAnswered: IClueModel[] =
              context.state.cluesAnswered || [];
            const clue = props.clue;
            const clueAnswered: IClueModel | undefined = cluesAnswered.find(
              (c) => c.id === props.clue.id
            );
            if (clueAnswered) {
              clueAnswered.answerAttempt = props.payload;
              clueAnswered.correct = correct;
            } else {
              clue.answerAttempt = props.payload;
              clue.correct = correct;
              cluesAnswered.push(clue);
            }
            context.commit(MutationTypes.SET_CLUES_ANSWERED, cluesAnswered);
          });
      }
      const p = new Promise<boolean>((resolve) => {
        resolve(correct);
      });
      Promise.resolve(p);
      return p;
    },
  },
  modules: {},
});
