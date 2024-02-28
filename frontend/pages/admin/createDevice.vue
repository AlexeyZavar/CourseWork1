<script lang="ts" setup>
const formValid = ref(false);
const showAlert = ref(false);
const alertError = ref();

const manufacturer = ref("");
const model = ref("");
const serial = ref("");

function stringRules(value: string) {
  return !!value || "Поле обязательно для заполнения";
}

async function create() {
  const { error, status } = await apiFetch<CreateDeviceResponse>(
    "/device/create",
    {
      method: "POST",
      body: JSON.stringify({
        manufacturer: manufacturer.value,
        model: model.value,
        serial: serial.value,
      }),
    },
  );

  if (status.value === "success") {
    await navigateTo("/admin/devices");
  } else {
    showAlert.value = true;
    alertError.value = error.value!.data.message;
  }
}

const rules = [stringRules];

useHead({
  title: "Добавление устройства",
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
            v-model="manufacturer"
            :rules="rules"
            class="mt-4"
            label="Производитель"
            name="manufacturer"
            required
            variant="outlined"
          ></v-text-field>
          <v-text-field
            v-model="model"
            :rules="rules"
            class="mt-4"
            label="Модель"
            name="model"
            required
            variant="outlined"
          ></v-text-field>
          <v-text-field
            v-model="serial"
            :rules="rules"
            class="mt-4"
            label="Серийный номер"
            name="serial"
            required
            variant="outlined"
          ></v-text-field>
          <v-btn :disabled="!formValid" size="large" @click="create">
            Добавить
          </v-btn>
        </v-form>
      </v-col>
    </v-row>
  </v-layout>
</template>

<style scoped></style>
