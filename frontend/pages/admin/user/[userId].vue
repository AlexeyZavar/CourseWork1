<script lang="ts" setup>
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
                  v-model="user.id"
                  label="ID"
                  readonly
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  v-model="user.firstName"
                  label="Имя"
                  @change="save"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  v-model="user.lastName"
                  label="Фамилия"
                  @change="save"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  v-model="user.userName"
                  label="Юзернейм"
                  @change="save"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  v-model="user.email"
                  label="Почта"
                  @change="save"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  v-model="user.telegram"
                  label="Telegram"
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
