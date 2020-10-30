import axios, { AxiosResponse } from "axios";
import { IGameModel } from "@/models/IGameModel";
import { ISubscriberModel } from "@/models/ISubscriberModel";
import { ITournamentModel } from "@/models/ITournamentModel";
import { SubscribeToTournamentModel } from "@/models/SubscribeToTournamentModel";

export class TournamentService {
  private controllerUrl: string;
  /**
   *
   */
  constructor() {
    this.controllerUrl = process.env.VUE_APP_API_BASE_URL + "/api/tournament";
  }
  /**
   * get a game
   */
  async getGame(
    tournamentid: string,
    id: string
  ): Promise<AxiosResponse<IGameModel>> {
    return axios.get(`${this.controllerUrl}/${tournamentid}/game/${id}`);
  }
  /**
   * get list of tournaments
   */
  async getTournaments(): Promise<AxiosResponse<ITournamentModel[]>> {
    return axios.get(this.controllerUrl);
  }
  /**
   * get a tournament
   */
  async getTournament(id: string): Promise<AxiosResponse<ITournamentModel>> {
    return axios.get(`${this.controllerUrl}/${id}`);
  }
  /**
   * subscribe to particular tournament
   */
  async subscribeToTournament(
    id: string,
    model: SubscribeToTournamentModel
  ): Promise<AxiosResponse<ISubscriberModel>> {
    return axios.post(`${this.controllerUrl}/${id}/subscriber`, model);
  }
  /**
   * unsubscribe from a particular tournament
   */
  async unsubscribeFromTournament(
    id: string,
    subscriberId: string
  ): Promise<AxiosResponse<void>> {
    return axios.delete(
      `${this.controllerUrl}/${id}/subscriber/${subscriberId}`
    );
  }
  /**
   * submit an answer attempt
   */
  async submitAnswer(
    tournamentId: string,
    subscriberId: string,
    gameId: string,
    clueId: string,
    answer: string
  ): Promise<AxiosResponse> {
    return axios.post(
      `${this.controllerUrl}/${tournamentId}/subscriber/${subscriberId}/game/${gameId}/clue/${clueId}/answerattempt`,
      { Answer: answer }
    );
  }
}
