<script setup lang="ts">
const formValid = ref(false);
const showAlert = ref(false);
const alertError = ref();

const userName = ref("alexeyzavar2");
const password = ref("1122");
const firstName = ref("Alexus");
const lastName = ref("Zavarchenko");
const birthDay = ref("");
const gender = ref("Мужчина");
const weight = ref(60);

function stringRules(value: string) {
  return !!value || "Поле обязательно для заполнения";
}

async function create() {
  const { error, status } = await apiFetch<CreatePatientResponse>(
    "/patient/create",
    {
      method: "POST",
      body: JSON.stringify({
        userName: userName.value,
        password: password.value,
        firstName: firstName.value,
        lastName: lastName.value,
        birthDay: birthDay.value,
        gender: gender.value === "Мужчина" ? 0 : 1,
        weight: weight.value,
      }),
    },
  );

  if (status.value === "success") {
    await navigateTo("/admin");
  } else {
    showAlert.value = true;
    alertError.value = error.value!.data.message;
  }
}

const rules = [stringRules];

useHead({
  title: "Добавление пациента",
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

          <v-text-field
            v-model="birthDay"
            name="birthDay"
            type="date"
            label="День рождения"
            variant="outlined"
            class="mt-4"
            required
            :rules="rules"
          ></v-text-field>
          <v-select
            v-model="gender"
            bg-color="bg"
            label="Пол"
            :items="['Мужчина', 'Женщина']"
            required
            :rules="rules"
          />
          <v-text-field
            v-model="weight"
            name="weight"
            min="40"
            max="300"
            type="number"
            label="Текущий вес"
            variant="outlined"
            class="mt-4"
            required
            :rules="rules"
          ></v-text-field>
          <v-btn size="large" @click="create" :disabled="!formValid">
            Добавить
          </v-btn>
        </v-form>
      </v-col>
    </v-row>
  </v-layout>
</template>

<style scoped></style>
