<script setup lang="ts">
const query = ref("");
const page = ref(1);

const { data: users } = await apiFetch<SearchUsersResponse>("/user/search", {
  query: {
    query: query,
  },
});

const headers = [
  { title: "ID", value: "id" },
  { title: "Юзернейм", value: "userName" },
  { title: "Имя", value: "firstName" },
  { title: "Фамилия", value: "lastName" },
  {
    title: "Роль",
    value: "role",
  },
];

useHead({
  title: "Список пользователей",
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
          :items="users.data"
          :items-per-page="10"
          class="elevation-1"
        >
          <template v-slot:item="{ item }">
            <tr @click="navigateTo('/admin/user/' + item.id)">
              <td>{{ item.id }}</td>
              <td>{{ item.userName }}</td>
              <td>{{ item.firstName }}</td>
              <td>{{ item.lastName }}</td>
              <td>
                {{
                  item.type === 0
                    ? "Пациент"
                    : item.type === 1
                      ? "Врач"
                      : "Администратор"
                }}
              </td>
            </tr>
          </template>
        </v-data-table>
      </v-col>
    </v-row>
  </v-layout>
</template>

<style scoped></style>
