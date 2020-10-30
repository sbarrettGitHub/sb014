<template>
  <div>
    <h1>
      Waitin for game to start
    </h1>
    <div>
      <unsubscribe />
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import unsubscribe from "@/components/shared/Unsubscribe.vue";
export default defineComponent({
  name: "NoPlay",
  components: {
    unsubscribe,
  },
});
</script>
