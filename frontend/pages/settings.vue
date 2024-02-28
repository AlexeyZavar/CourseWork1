<script setup lang="ts">
const { getSession } = useAuth();

const session = await getSession();

const email = ref(session!.email);
const telegram = ref(session!.telegram);

async function save() {
  const { error, status } = await apiFetch(`/user/${session!.id}/update`, {
    method: "POST",
    body: JSON.stringify({
      email: email.value,
      telegram: telegram.value,
    }),
  });

  if (status.value === "success") {
    await getSession({ force: true });
    await refreshNuxtData();
  } else {
    console.error(error);
  }
}

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
          v-model="email"
        ></v-text-field>
        <v-text-field
          label="Telegram"
          variant="outlined"
          class="mt-4"
          v-model="telegram"
        ></v-text-field>
        <v-btn size="large" @click="save"> Сохранить </v-btn>
      </v-col>
    </v-row>
  </v-layout>
</template>

<style scoped></style>
