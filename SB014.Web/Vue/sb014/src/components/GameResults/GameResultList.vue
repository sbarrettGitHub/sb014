<template>
  <div>{{ gameResultList }}</div>
</template>
<script lang="ts">
import { defineComponent, ref } from "vue";
export default defineComponent({
  name: "gameResultList",
  setup() {
    const gameResultList = ref<string>("Game result list ...");
    return {
      gameResultList,
    };
  },
});
</script>
