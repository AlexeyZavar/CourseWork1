<script lang="ts" setup>
const { data: user } = useAuth();

onMounted(async () => {
  if (user.value!.patients.length === 1) {
    const patientId = user.value!.patients[0].id;
    await navigateTo(`/patients/${patientId}`);
    return;
  }
});
</script>

<template></template>

<style scoped></style>
