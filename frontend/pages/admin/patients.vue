<script lang="ts" setup>
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
          :items="patients.data"
          :items-per-page="10"
          :page="page"
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
