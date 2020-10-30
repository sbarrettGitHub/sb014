<template>
  <div class="home">
    <h1>Home here...</h1>
    <label for="name">Name</label>
    <input type="text" name="name" v-model="name" />
    <label for="name">Country</label>
    <input type="text" name="countryCode" v-model="countryCode" />
    <tournament-list
      v-if="!isLoading"
      :tournaments="tournaments"
      :name="name"
      :country-code="countryCode"
    />
  </div>
</template>

<script lang="ts">
import { ActionTypes } from "@/store";
import { computed, defineComponent, onMounted, ref } from "vue";
import { useHubConnection } from "@/notifications/hubHelpers";
import { useStore } from "vuex";
import tournamentList from "@/components/Home/TournamentList.vue"; // @ is an alias to /src

export default defineComponent({
  name: "Home",
  components: {
    tournamentList,
  },
  setup() {
    const store = useStore();
    const hubConnection = useHubConnection();
    const isLoading = ref<boolean>(false);
    const name = ref<string>("");
    const countryCode = ref<string>("");
    const tournaments = computed(() => {
      return store.getters.tournaments;
    });

    onMounted(async () => {
      isLoading.value = true;
      //debugger;// eslint-disable-line
      const tournamentId = store.getters.currentTournament?.id;
      // unsubscribe to tournament notifications
      if (hubConnection && tournamentId) {
        await hubConnection.invoke(
          "UnsubscribeFromTournamentNotifications",
          tournamentId
        );
        //await hubConnection.stop();
      }

      // Unscubscribe from any tournament already subscribed to
      await store.dispatch(ActionTypes.UNSUBSCRIBE_FROM_TOURNAMENT);

      // Load all tournaments
      await store.dispatch(ActionTypes.LOAD_TOURNAMENTS);
      if (localStorage.name) {
        name.value = localStorage.name;
      }
      if (localStorage.countryCode) {
        countryCode.value = localStorage.countryCode;
      }
      isLoading.value = false;
    });

    return {
      countryCode,
      isLoading,
      name,
      tournaments,
    };
  },
});
</script>
