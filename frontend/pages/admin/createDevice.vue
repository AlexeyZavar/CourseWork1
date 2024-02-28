<script setup lang="ts">
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
          variant="tonal"
          closable
          color="error"
          title="Ошибка"
        >
          {{ alertError }}
        </v-alert>
        <v-form v-model="formValid">
          <v-text-field
            v-model="manufacturer"
            name="manufacturer"
            label="Производитель"
            variant="outlined"
            class="mt-4"
            required
            :rules="rules"
          ></v-text-field>
          <v-text-field
            v-model="model"
            name="model"
            label="Модель"
            variant="outlined"
            class="mt-4"
            required
            :rules="rules"
          ></v-text-field>
          <v-text-field
            v-model="serial"
            name="serial"
            label="Серийный номер"
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
