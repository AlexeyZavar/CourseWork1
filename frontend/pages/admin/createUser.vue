<script setup lang="ts">
const formValid = ref(false);
const showAlert = ref(false);
const alertError = ref();

const userName = ref("alexeyzavar2");
const password = ref("1122");
const firstName = ref("Alexus");
const lastName = ref("Zavarchenko");
const type = ref("Врач");

function stringRules(value: string) {
  return !!value || "Поле обязательно для заполнения";
}

async function create() {
  const { error, status } = await apiFetch<CreateUserResponse>("/user/create", {
    method: "POST",
    body: JSON.stringify({
      userName: userName.value,
      password: password.value,
      firstName: firstName.value,
      lastName: lastName.value,
      type: type.value === "Врач" ? 1 : 2,
    }),
  });

  if (status.value === "success") {
    await navigateTo("/admin/users");
  } else {
    showAlert.value = true;
    alertError.value = error.value!.data.message;
  }
}

const rules = [stringRules];

useHead({
  title: "Добавление пользователя",
});
</script>

<template>
  <v-layout>
    <v-row>
      <v-col>
        <v-alert
          v-model="showAlert"
          border="start"
          variant="tonal"
          closable
          color="error"
          title="Ошибка"
        >
          {{ alertError }}
        </v-alert>
        <v-form v-model="formValid">
          <v-text-field
            v-model="userName"
            name="userName"
            label="Юзернейм"
            variant="outlined"
            class="mt-4"
            required
            :rules="rules"
          ></v-text-field>
          <v-text-field
            v-model="password"
            name="password"
            label="Пароль"
            variant="outlined"
            class="mt-4"
            required
            :rules="rules"
          ></v-text-field>
          <v-spacer />
          <v-text-field
            v-model="firstName"
            name="firstName"
            label="Имя"
            variant="outlined"
            class="mt-4"
            required
            :rules="rules"
          ></v-text-field>
          <v-text-field
            v-model="lastName"
            name="lastName"
            label="Фамилия"
            variant="outlined"
            class="mt-4"
            required
            :rules="rules"
          ></v-text-field>

          <v-select
            v-model="type"
            bg-color="bg"
            label="Роль"
            :items="['Врач', 'Администратор']"
            required
            :rules="rules"
          />
          <v-btn size="large" @click="create" :disabled="!formValid">
            Добавить
          </v-btn>
        </v-form>
      </v-col>
    </v-row>
  </v-layout>
</template>

<style scoped></style>
