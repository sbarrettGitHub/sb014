<template>
  <router-view />
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { ITournamentModel } from "./models/ITournamentModel";
import { useHubConnection } from "@/notifications/hubHelpers";
import { useStore } from "vuex";
import { useRouter } from "vue-router";
import { TournamentState } from "./models/enums/TournamentState";
import { MutationTypes } from "./store";

export default defineComponent({
  setup() {
    const hubConnection = useHubConnection();
    const router = useRouter();
    const store = useStore();
    if (hubConnection) {
      // Control game flow via tournament updates messages from the server
      hubConnection.on(
        "TournamentUpdate",
        (tournamentUpdate: ITournamentModel) => {
          console.log(tournamentUpdate);
          store.commit(MutationTypes.SET_CURRENT_TOURNAMENT, tournamentUpdate);
          // Depending on the tournament state switch views
          switch (tournamentUpdate.state) {
            case TournamentState.NoPlay:
              router.push("/game/noplay");
              break;
            case TournamentState.InPlay:
              router.push("/game/inplay");
              break;
            case TournamentState.PostPlay:
              router.push("/game/postplay");
              break;
            default:
              break;
          }
        }
      );
    }
  },
});
</script>
<style lang="scss"></style>
