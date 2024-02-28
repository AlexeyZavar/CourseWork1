<script setup lang="ts">
const route = useRoute();

const { data: user } = await apiFetch<UserDto>(`/user/${route.params.userId}`);

async function save() {
  await apiFetch(`/user/${user.value.id}/update`, {
    method: "POST",
    body: JSON.stringify(user.value),
  });

  await refreshNuxtData();
}

useHead({
  title: "Данные о пользователе",
});
</script>

<template>
  <v-layout>
    <v-row>
      <v-col>
        <v-card>
          <v-card-title>
            <span class="title">Информация о пользователе</span>
          </v-card-title>
          <v-card-text>
            <v-row>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="ID"
                  v-model="user.id"
                  readonly
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Имя"
                  v-model="user.firstName"
                  @change="save"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Фамилия"
                  v-model="user.lastName"
                  @change="save"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Юзернейм"
                  v-model="user.userName"
                  @change="save"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Почта"
                  v-model="user.email"
                  @change="save"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Telegram"
                  v-model="user.telegram"
                  @change="save"
                ></v-text-field>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-layout>
</template>

<style scoped></style>
