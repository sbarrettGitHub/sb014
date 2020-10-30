<template>
  <div>
    <h1>
      Game here ...
    </h1>
    <div>
      <unsubscribe />
    </div>
    <div>
      Remaining clues: {{remainingClues.length}}
    </div>
    <clue :clue="currentClue" />
    <div>Clue: {{ currentClue ? currentClue.gameClue : "no clue" }}</div>
    <div>Clue No: {{ currentClueIndex + 1 }}</div>
    <button @click="moveToPreviousClue">
      &lt;- previous
    </button>
    <button @click="moveToNextClue">
      next -&gt;
    </button>

    <div>
     <clues-answered/>
    </div>
  </div>
</template>
<script lang="ts">
import clue from "@/components/Game/Clue.vue";
import { computed, defineComponent, onMounted, ref, watch } from "vue";
import { ActionTypes } from "@/store";
import { IClueModel } from "@/models/IClueModel";
import { IGameModel } from "@/models/IGameModel";
import unsubscribe from "@/components/shared/Unsubscribe.vue";
import cluesAnswered from "@/components/shared/CluesAnswered.vue";
import { useStore } from "vuex";
import { useRouter } from 'vue-router';
export default defineComponent({
  name: "InPlay",
  components: {
    clue,
    cluesAnswered,
    unsubscribe,
  },
  setup() {
    const store = useStore();
    const router = useRouter();
    const currentClueIndex = ref<number>(0);
    const isLoading = ref<boolean>(false);
    const cluesAnswered = computed((): IClueModel[] => {
      return store.getters.cluesAnswered;
    });
    const currentGame = computed((): IGameModel | null => {
      return store.getters.currentInPlayGame;
    });
    const remainingClues = computed((): IClueModel[] => {
      if (
        currentGame.value &&
        currentGame.value.clues &&
        currentGame.value.clues.length
      ) {
        return currentGame.value.clues.filter((c: IClueModel) => {
          if (
            store.getters.cluesAnswered.find((clue: IClueModel) => c.id === clue.id && clue.correct == true)) {
            return false;
          } else {
            return true;
          }
        });
      } else {
        return [];
      }
    });
    const currentClue = computed((): IClueModel | null => {
      if (remainingClues.value.length >= currentClueIndex.value + 1) {
        return remainingClues.value[currentClueIndex.value];
      }
      return null;
    });
    function moveToNextClue() {
      if (
        remainingClues.value.length
      ) {
        if (remainingClues.value.length > currentClueIndex.value + 2) {
          currentClueIndex.value++;
        } else {
          currentClueIndex.value = 0;
        }
      }
    }
    function moveToPreviousClue() {
      if (
        remainingClues.value.length
      ) {
        if (currentClueIndex.value > 0) {
          currentClueIndex.value--;
        } else {
          currentClueIndex.value = remainingClues.value.length - 1;
        }
      }
    }
    watch(remainingClues, (newValue) => {
      if(!isLoading.value && newValue.length == 0){
        router.push("/game/finished");
      }
    });
    onMounted(async () => {
      isLoading.value = true;
      await store.dispatch(ActionTypes.LOAD_CURRENT_INPLAY_GAME);
      isLoading.value = false;
    });

    return {
      cluesAnswered,
      currentClue,
      currentClueIndex,
      isLoading,
      moveToPreviousClue,
      moveToNextClue,
      remainingClues
    };
  },
});
</script>
