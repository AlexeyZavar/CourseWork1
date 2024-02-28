<script lang="ts" setup>
const route = useRoute();

const initial = new Date();
initial.setDate(initial.getDate() - 7);

const dataFrom = ref(initial.toISOString().slice(0, 10));
const dataTo = ref(new Date().toISOString().slice(0, 10));

const dataFromComputed = computed(() => {
  const date = new Date(dataFrom.value);
  date.setHours(0, 0, 0, 0);
  return date.toISOString();
});
const dataToComputed = computed(() => {
  const date = new Date(dataTo.value);
  date.setHours(23, 59, 59, 999);
  return date.toISOString();
});

const warningsPage = ref(1);

const { data: patient } = await apiFetch<PatientDto>(
  `/patient/${route.params.patientId}`,
);
const { data: measurements, refresh: refreshMeasurements } =
  await apiFetch<PatientMeasurements>(
    `/patient/${route.params.patientId}/data`,
    {
      query: {
        from: dataFromComputed,
        to: dataToComputed,
      },
    },
  );
const { data: warnings, refresh: refreshWarnings } =
  await apiFetch<PatientWarnings>(
    `/patient/${route.params.patientId}/warnings`,
  );

const name = computed(
  () => patient.value.firstName + " " + patient.value.lastName,
);
const subtitle = computed(() => {
  const gender = patient.value.gender === 0 ? "Мужчина" : "Женщина";
  const age = getPluralAge(patient.value.age);
  const weight = patient.value.weight + " кг";

  return `${gender}, ${age}, вес ${weight}`;
});

const measureDataHeaders = [
  {
    title: "Время",
    key: "time",
    value: (item: MeasureDataDto) => formatDateTime(item.time),
  },
  {
    title: "Верхнее арт. давление",
    key: "sys",
    value: (item: MeasureDataDto) => item.sys + " ммрт",
  },
  {
    title: "Нижнее арт. давление",
    key: "dia",
    value: (item: MeasureDataDto) => item.dia + " ммрт",
  },
  {
    title: "Пульс",
    key: "pulse",
    value: (item: MeasureDataDto) => item.pulse + " уд/мин",
  },
];

async function exportMeasurements() {
  const { data: file } = await apiFetch<string>(
    `/patient/${route.params.patientId}/export`,
    {
      query: {
        format: "json",
      },
    },
  );

  const blob = new Blob([file.value], { type: "application/json" });
  const url = URL.createObjectURL(blob);
  const a = document.createElement("a");
  a.href = url;
  a.download = "measurements.json";
  a.click();
}

const refresherTask = ref();

onMounted(() => {
  refresherTask.value = setInterval(() => {
    refreshMeasurements();
    refreshWarnings();
  }, 3500);
});

onUnmounted(() => {
  clearInterval(refresherTask.value);
});

useHead({
  title: "Данные о пациенте",
});
</script>

<template>
  <div>
    <v-row>
      <v-col>
        <v-card>
          <v-card-title class="text-h4"> {{ name }}</v-card-title>
          <v-card-subtitle class="!tw-pb-2">{{ subtitle }}</v-card-subtitle>
        </v-card>
      </v-col>
      <v-col>
        <v-card>
          <v-card-title class="text-h4"> Предупреждения</v-card-title>
          <v-data-iterator
            :items="warnings.data"
            :page="warningsPage"
            class="tw-p-4"
          >
            <template v-slot:default="{ items }">
              <template v-for="(item, i) in items" :key="i">
                <v-card>
                  <v-card-title>{{ item.raw.message }}</v-card-title>
                  <v-card-subtitle class="pb-2"
                    >{{ formatDateTime(item.raw.time) }}
                  </v-card-subtitle>
                </v-card>
                <br v-if="i % items.length != items.length - 1" />
              </template>
            </template>
          </v-data-iterator>
        </v-card>
      </v-col>
    </v-row>
    <v-card class="tw-mt-6">
      <v-row class="mt-4" justify="center">
        <v-text-field v-model="dataFrom" type="date" />
        <v-text-field v-model="dataTo" type="date" />
      </v-row>
      <v-data-table
        :headers="measureDataHeaders"
        :items="measurements.data"
      ></v-data-table>
      <v-btn @click="exportMeasurements">Экспорт</v-btn>
    </v-card>
  </div>
</template>

<style scoped></style>
