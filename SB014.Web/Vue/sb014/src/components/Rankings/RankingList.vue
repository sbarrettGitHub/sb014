<template>
  <div>{{rankingList}}</div>
</template>
<script lang="ts">
import { defineComponent, ref } from "vue";

export default defineComponent({
  name: "rankingList",
  setup() {
      const rankingList = ref<string>("ranking list");
    return {
        rankingList
    };
  },
});
</script>
