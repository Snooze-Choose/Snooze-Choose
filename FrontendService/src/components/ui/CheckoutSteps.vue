<script setup lang="ts">
import { Progress } from '@/components/ui/progress';
import { defineProps, ref, watch } from 'vue';

// Prop für den Fortschritt
const props = defineProps<{ progressValue: number }>();
const progressReactive = ref(0);

// Aktualisiere `progressReactive` mit Verzögerung, um Vue zu triggern
watch(() => props.progressValue, (newValue) => {
  setTimeout(() => {
    progressReactive.value = newValue;
  }, 50);
});
</script>

<template>
  <div class="mb-6">
    <p class="text-sm font-medium text-gray-700 mb-2">
      Checkout-Fortschritt: {{ progressReactive }}%
    </p>
    <Progress :value="progressReactive" class="w-full h-2 bg-gray-200" />
  </div>
</template>
