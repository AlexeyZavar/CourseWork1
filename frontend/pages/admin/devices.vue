<script lang="ts" setup>
const query = ref("");
const page = ref(1);

const { data: devices } = await apiFetch<SearchDevicesResponse>(
  "/device/search",
  {
    query: {
      query: query,
    },
  },
);

const headers = [
  { title: "Производитель", value: "manufacturer" },
  { title: "Модель", value: "model" },
  { title: "Серийный номер", value: "serial" },
];

useHead({
  title: "Список устройств",
});
</script>

<template>
  <v-layout>
    <v-row>
      <v-col>
        <v-text-field
          v-model="query"
          class="mt-4"
          hide-details
          label="Поиск"
          prepend-inner-icon="mdi-magnify"
          single-line
          variant="outlined"
        ></v-text-field>
        <v-data-table
          :headers="headers"
          :items="devices.data"
          :items-per-page="10"
          :page="page"
          class="elevation-1"
        >
          <template v-slot:item="{ item }">
            <tr @click="navigateTo('/admin/device/' + item.id)">
              <td>{{ item.manufacturer }}</td>
              <td>{{ item.model }}</td>
              <td class="tw-font-mono">{{ item.serial }}</td>
            </tr>
          </template>
        </v-data-table>
      </v-col>
    </v-row>
  </v-layout>
</template>

<style scoped></style>
