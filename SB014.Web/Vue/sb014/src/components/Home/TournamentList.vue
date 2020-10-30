<template>
  <div>
    {{ tournaments.length + " tournaments available" }}
    <div v-for="t in tournaments" :key="t.id">
      <label for="">Tournament: {{ t.id }}</label>
      <button @click="subscribeToTournament(t)">
        Subscribe
      </button>
    </div>
  </div>
</template>
<script lang="ts">
import { ActionTypes } from "@/store";
import { defineComponent } from "vue";
import { ITournamentModel } from "@/models/ITournamentModel";
import { useStore } from "vuex";
import { SubscribeToTournamentModel } from "@/models/SubscribeToTournamentModel";
import { TournamentState } from "@/models/enums/TournamentState";
import { useRouter } from "vue-router";
import { useHubConnection } from "@/notifications/hubHelpers"
export default defineComponent({
  name: "tournamentList",
  props: {
    name: String,
    countryCode: String,
    tournaments: {
      type: Object as () => ITournamentModel[],
      required: true,
    },
  },
  setup(props) {
    const store = useStore();
    const router = useRouter();
    const hubConnection = useHubConnection();

    async function subscribeToTournament(t: ITournamentModel) {
      const subscribeModel = new SubscribeToTournamentModel();
      subscribeModel.Name = props.name || "unknown";
      subscribeModel.CountryCode = props.countryCode || "unknown";

      // Subscribe
      await store.dispatch(ActionTypes.SUBSCRIBE_TO_TOURNAMENT, {
        id: t.id,
        payload: subscribeModel,
      });

      // load subscribed tournament
      await store.dispatch(ActionTypes.LOAD_CURRENT_TOURNAMENT);

      // subscribe to tournament notifications
      if(hubConnection){
        hubConnection.invoke("SubscribeToTournamentNotifications", t.id);
      }
      // Persist name and country in local storage
      localStorage.name = subscribeModel.Name;
      localStorage.countryCode = subscribeModel.CountryCode;
      
      // Depending on the tournament state switch views
      switch (store.getters.currentTournament.state) {
        case TournamentState.NoPlay:
          router.push("/game/noplay");
          break;
        case TournamentState.InPlay:
          router.push("/game/inplay");
          break;
        case TournamentState.PostPlay:
        case TournamentState.PrePlay:
          router.push("/game/postplay");
          break;
        default:
          break;
      }
    }
    return {
      subscribeToTournament,
    };
  },
});
</script>
