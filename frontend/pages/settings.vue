<script lang="ts" setup>
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
          v-model="email"
          class="mt-4"
          label="Почта"
          variant="outlined"
        ></v-text-field>
        <v-text-field
          v-model="telegram"
          class="mt-4"
          label="Telegram"
          variant="outlined"
        ></v-text-field>
        <v-btn size="large" @click="save"> Сохранить</v-btn>
      </v-col>
    </v-row>
  </v-layout>
</template>

<style scoped></style>
