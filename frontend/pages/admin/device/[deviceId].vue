<script setup lang="ts">
const route = useRoute();

const { data: device } = await apiFetch<MeasureDeviceDto>(
  `/device/${route.params.deviceId}`,
);

useHead({
  title: "Данные об устройстве",
});
</script>

<template>
  <v-layout>
    <v-row>
      <v-col>
        <v-card>
          <v-card-title>
            <span class="title">Информация об устройстве</span>
          </v-card-title>
          <v-card-text>
            <v-row>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="ID"
                  v-model="device.id"
                  readonly
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Производитель"
                  v-model="device.manufacturer"
                  readonly
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Модель"
                  v-model="device.model"
                  readonly
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  label="Серийный номер"
                  v-model="device.serial"
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
