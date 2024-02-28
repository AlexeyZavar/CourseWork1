<script setup lang="ts">
const { data: stats } = await apiFetch<StatisticsResponse>("/admin/statistics");

useHead({
  title: "Статистика по серверу",
});
</script>

<template>
  <v-layout>
    <v-row>
      <v-col>
        <v-card>
          <v-card-title>
            <span class="title">Статистика по серверу</span>
          </v-card-title>
          <v-card-text>
            <v-row>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Количество пациентов"
                  v-model="stats.totalPatients"
                  readonly
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Количество измерений"
                  v-model="stats.totalMeasurements"
                  readonly
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Количество устройств"
                  v-model="stats.totalDevices"
                  readonly
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Количество предупреждений"
                  v-model="stats.totalWarnings"
                  readonly
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Количество пользователей"
                  v-model="stats.totalUsers"
                  readonly
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Количество смотрящих"
                  v-model="stats.totalDoctors"
                  readonly
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Количество администраторов"
                  v-model="stats.totalAdmins"
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
