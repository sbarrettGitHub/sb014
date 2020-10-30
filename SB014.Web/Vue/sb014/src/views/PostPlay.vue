<template>
  <div>
    <h1>
      Game results and rankings here ...
    </h1>
    <div>
      <unsubscribe />
    </div>
    <game-result-list/>
    <ranking-list/>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import gameResultList from '@/components/GameResults/GameResultList.vue'
import rankingList from "../components/Rankings/RankingList.vue";
import unsubscribe from "@/components/shared/Unsubscribe.vue";

export default defineComponent({
  name: "PostPlay",
  components: {
      gameResultList,
      rankingList,
      unsubscribe,
  },
});
</script>
