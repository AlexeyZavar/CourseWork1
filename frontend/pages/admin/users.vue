<script lang="ts" setup>
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
          :items="users.data"
          :items-per-page="10"
          :page="page"
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
