<template>  
    <div v-for="clue in cluesAnswered" :key="clue.id">
        Clue:"{{clue.gameClue}}" - Your answer: "{{clue.answerAttempt}}" - {{clue.correct == true ? 'Correct :)': 'Incorrect :('}}
    </div>  
</template>
<script lang="ts">
import { computed, defineComponent } from "vue";
import { useStore } from 'vuex';
import { IClueModel } from '@/models/IClueModel';
export default defineComponent({    
 setup(){
    const store = useStore();
    const cluesAnswered = computed((): IClueModel[] => {
      return store.getters.cluesAnswered;
    });
    return{
      cluesAnswered
    }
  }
});
</script>
