<template>
  <div>
    <button @click="unsubscribe">
      Exit
    </button>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { useRouter } from 'vue-router';

export default defineComponent({
  setup() {
      const router = useRouter()
    function unsubscribe() {
        if(confirm("Are you sure you wish to quit?")){
            router.push("/");
        }
    }
    return {
      unsubscribe,
    };
  },
});
</script>
