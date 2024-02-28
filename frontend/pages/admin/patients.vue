<script setup lang="ts">
const query = ref("");
const page = ref(1);

const { data: patients } = await apiFetch<SearchUsersResponse>(
  "/patient/search",
  {
    query: {
      query: query,
    },
  },
);

const headers = [
  { title: "ID", value: "id" },
  { title: "Имя", value: "firstName" },
  { title: "Фамилия", value: "lastName" },
  { title: "Возраст", value: "age" },
  { title: "Вес", value: "weight" },
];

useHead({
  title: "Список пациентов",
});
</script>

<template>
  <v-layout>
    <v-row>
      <v-col>
        <v-text-field
          prepend-inner-icon="mdi-magnify"
          single-line
          label="Поиск"
          v-model="query"
          variant="outlined"
          hide-details
          class="mt-4"
        ></v-text-field>
        <v-data-table
          :page="page"
          :headers="headers"
          :items="patients.data"
          :items-per-page="10"
          class="elevation-1"
        >
          <template v-slot:item="{ item }">
            <tr @click="navigateTo('/patients/' + item.id)">
              <td>{{ item.id }}</td>
              <td>{{ item.firstName }}</td>
              <td>{{ item.lastName }}</td>
              <td>{{ getPluralAge(item.age) }}</td>
              <td>{{ item.weight }} кг</td>
            </tr>
          </template>
        </v-data-table>
      </v-col>
    </v-row>
  </v-layout>
</template>

<style scoped></style>
