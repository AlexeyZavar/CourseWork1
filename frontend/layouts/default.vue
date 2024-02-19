<script lang="ts" setup>
const { signOut, getSession, data: session } = useAuth();
const isAdmin = useIsAdmin();

async function logout() {
  await signOut({ callbackUrl: "/" });
  await navigateTo("/login");
}

await getSession({ force: true });

// инициалы
const initials = computed(
  () =>
    (session.value?.firstName[0] ?? "") + (session.value?.lastName[0] ?? ""),
);

useHead({
  title: "Главная",
});
</script>

<template>
  <v-app>
    <v-layout>
      <v-app-bar flat>
        <v-container class="mx-auto d-flex align-center justify-center">
          <v-btn text="Главная" variant="text" @click="navigateTo('/')" />
          <v-btn
            text="Управление"
            variant="text"
            @click="navigateTo('/admin')"
            v-if="isAdmin"
          />

          <v-spacer></v-spacer>

          <v-avatar class="me-4 tw-select-none" color="grey-darken-1" size="36">
            {{ initials }}
            <v-menu
              activator="parent"
              location="bottom center"
              origin="top center"
            >
              <v-list rounded="lg">
                <v-list-item link @click="navigateTo('/settings')">
                  <v-list-item-title>Настройки</v-list-item-title>
                </v-list-item>
                <v-divider class="my-1" />
                <v-list-item link @click="logout">
                  <v-list-item-title>Выйти</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
          </v-avatar>
        </v-container>
      </v-app-bar>

      <v-main class="bg-grey-lighten-3">
        <v-container>
          <v-row>
            <v-col>
              <v-sheet rounded="lg" class="px-6 pt-2 pb-6">
                <v-divider class="mt-2" />
                <div>
                  <slot />
                </div>
              </v-sheet>
            </v-col>
          </v-row>
        </v-container>
      </v-main>
    </v-layout>
  </v-app>
</template>

<style scoped></style>
