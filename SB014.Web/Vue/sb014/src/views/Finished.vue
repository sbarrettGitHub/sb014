<template>
  <div>
    <h1>
      All clues solved !!!
    </h1>
    <div>
      <unsubscribe />
    </div>
    <clues-answered />
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import cluesAnswered from "@/components/shared/CluesAnswered.vue";
import unsubscribe from "@/components/shared/Unsubscribe.vue";
export default defineComponent({
  name: "NoPlay",
  components: {
    cluesAnswered,
    unsubscribe,
  },
});
</script>
