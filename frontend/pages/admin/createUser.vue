<script lang="ts" setup>
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
          closable
          color="error"
          title="Ошибка"
          variant="tonal"
        >
          {{ alertError }}
        </v-alert>
        <v-form v-model="formValid">
          <v-text-field
            v-model="userName"
            :rules="rules"
            class="mt-4"
            label="Юзернейм"
            name="userName"
            required
            variant="outlined"
          ></v-text-field>
          <v-text-field
            v-model="password"
            :rules="rules"
            class="mt-4"
            label="Пароль"
            name="password"
            required
            variant="outlined"
          ></v-text-field>
          <v-spacer />
          <v-text-field
            v-model="firstName"
            :rules="rules"
            class="mt-4"
            label="Имя"
            name="firstName"
            required
            variant="outlined"
          ></v-text-field>
          <v-text-field
            v-model="lastName"
            :rules="rules"
            class="mt-4"
            label="Фамилия"
            name="lastName"
            required
            variant="outlined"
          ></v-text-field>

          <v-select
            v-model="type"
            :items="['Врач', 'Администратор']"
            :rules="rules"
            bg-color="bg"
            label="Роль"
            required
          />
          <v-btn :disabled="!formValid" size="large" @click="create">
            Добавить
          </v-btn>
        </v-form>
      </v-col>
    </v-row>
  </v-layout>
</template>

<style scoped></style>
