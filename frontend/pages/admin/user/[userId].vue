<script setup lang="ts">
const route = useRoute();

const { data: user } = await apiFetch<UserDto>(`/user/${route.params.userId}`);

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
                  readonly
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Фамилия"
                  v-model="user.lastName"
                  readonly
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Юзернейм"
                  v-model="user.userName"
                  readonly
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Роль"
                  v-model="user.type"
                  readonly
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
