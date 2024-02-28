<script lang="ts" setup>
definePageMeta({
  layout: "empty",
  auth: {
    unauthenticatedOnly: true,
    navigateAuthenticatedTo: "/",
  },
});

const { signIn } = useAuth();

async function login() {
  await signIn(
    {
      username: username.value,
      password: password.value,
    },
    {
      redirect: true,
      callbackUrl: "/",
    },
  );
}

const username = ref("");
const password = ref("");

useHead({
  title: "Авторизация",
});
</script>

<template>
  <v-layout full-height>
    <v-main>
      <v-container class="h-screen">
        <v-row align="center" align-content="center" class="h-screen">
          <v-col cols="12" md="6" offset-md="3">
            <v-card>
              <v-card-title>
                <span class="headline">Вход</span>
              </v-card-title>

              <v-card-item>
                <div>
                  <v-text-field
                    v-model="username"
                    label="Логин"
                    name="username"
                    prepend-icon="mdi-account"
                    type="text"
                  ></v-text-field>

                  <v-text-field
                    v-model="password"
                    label="Пароль"
                    name="password"
                    prepend-icon="mdi-lock"
                    type="password"
                    @keydown.enter="login"
                  ></v-text-field>
                </div>
              </v-card-item>

              <v-card-actions>
                <v-spacer></v-spacer>

                <v-btn color="primary" depressed @click="login"> Войти</v-btn>
              </v-card-actions>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
    </v-main>
  </v-layout>
</template>

<style scoped></style>
