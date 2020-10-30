<template>
  <div>
    <div>
      <input type="text" v-model="answer" :maxlength="letters.length" />
    </div>
    <div class="lettersContainer">
      <div class="letter" v-for="l in letters" :key="l.no">
        {{ l.letter }}
      </div>
    </div>
    <div>
      <strong>{{feedback}}</strong>
    </div>
  </div>
</template>
<script lang="ts">
import { IClueLetter } from "@/models/IClueLetter";
import { IClueModel } from "@/models/IClueModel";
import { ActionTypes } from "@/store";
import { computed, defineComponent, PropType, ref, watch } from "vue";
import { useStore } from "vuex";

export default defineComponent({
  name: "clue",
  props: {
    clue: {
      type: Object as PropType<IClueModel>,
    },
  },
  setup(props) {
    const store = useStore();
    const clueModel = ref<string>("Clue rendered ...");
    const letters = computed((): IClueLetter[] => {
      const ls: string[] = props.clue ? props.clue.gameClue.split("") : [];
      return ls.map((l: string, i: number) => {
        return {
          no: i,
          letter: l,
        };
      });
    });
    const answer = ref<string>("");
    const feedback = ref<string>("");
    async function submitAnswer() {
      await store
        .dispatch(ActionTypes.SUBMIT_ANSWER, {
          clue: props.clue,
          payload: answer.value,
        })
        .then((isCorrect: boolean) => {
          if (isCorrect) {
            feedback.value = "Correct!";
            
          }else{
            feedback.value = "Not correct ... sorry :(";
          }
          setTimeout(()=>{
            feedback.value = "";
          }, 2000);
        });
    }
    watch(answer, (newValue) => {
      if (newValue.length === letters.value.length) {
        submitAnswer();
      }
    });
    watch(letters, () => {
      answer.value = "";
      feedback.value = "";
    });
    return {
      answer,
      clueModel,
      feedback,
      letters,
    };
  },
});
</script>
<style lang="scss" scoped>
.lettersContainer {
  display: flex;
  .letter {
    border: 1px solid black;
    padding: 4px;
    margin: 4px;
    font-size: x-large;
    min-width: 25px;
    min-height: 25px;
    text-align: center;
  }
}
</style>
