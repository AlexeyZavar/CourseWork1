<script setup lang="ts">
useHead({
  title: "Настройки",
});
</script>

<template>
  <v-layout>
    <v-row>
      <v-col>
        <v-text-field
          label="Почта"
          variant="outlined"
          class="mt-4"
        ></v-text-field>
        <v-text-field
          label="Telegram"
          variant="outlined"
          class="mt-4"
        ></v-text-field>
        <v-btn size="large"> Сохранить </v-btn>
      </v-col>
    </v-row>
  </v-layout>
</template>

<style scoped></style>
