<script setup lang="ts">
useHead({
  title: "Панель управления",
});
</script>

<template>
  <v-layout>
    <v-row>
      <v-col>
        <v-card>
          <v-card-title>
            <span class="title">Панель управления</span>
          </v-card-title>
          <v-card-text>
            <v-row>
              <v-col cols="12" sm="6">
                <v-btn
                  block
                  color="primary"
                  @click="navigateTo('/admin/users')"
                >
                  Пользователи
                </v-btn>
                <v-btn
                  block
                  color="primary"
                  @click="navigateTo('/admin/createUser')"
                >
                  Добавить пользователя
                </v-btn>
              </v-col>
              <v-col cols="12" sm="6">
                <v-btn
                  block
                  color="primary"
                  @click="navigateTo('/admin/patients')"
                >
                  Пациенты
                </v-btn>
                <v-btn
                  block
                  color="primary"
                  @click="navigateTo('/admin/createPatient')"
                >
                  Добавить пациента
                </v-btn>
              </v-col>
            </v-row>
            <v-row>
              <v-col cols="12" sm="6">
                <v-btn
                  block
                  color="primary"
                  @click="navigateTo('/admin/devices')"
                >
                  Устройства
                </v-btn>
                <v-btn
                  block
                  color="primary"
                  @click="navigateTo('/admin/createDevice')"
                >
                  Добавить устройство
                </v-btn>
              </v-col>
              <v-col cols="12" sm="6">
                <v-btn
                  block
                  color="primary"
                  @click="navigateTo('/admin/stats')"
                >
                  Статистика по серверу
                </v-btn>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-layout>
</template>

<style scoped></style>
