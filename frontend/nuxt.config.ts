import vuetify, { transformAssetUrls } from "vite-plugin-vuetify";

export default defineNuxtConfig({
  devtools: { enabled: true },
  runtimeConfig: {
    public: {
      apiURL: "http://localhost:5003",
    },
  },
  build: {
    transpile: ["vuetify"],
  },
  modules: [
    (_options, nuxt) => {
      nuxt.hooks.hook("vite:extendConfig", (config) => {
        // @ts-expect-error
        config.plugins.push(vuetify({ autoImport: true }));
      });
    },
    "@sidebase/nuxt-auth",
    "@nuxtjs/tailwindcss",
  ],
  vite: {
    vue: {
      template: {
        transformAssetUrls,
      },
    },
  },
  auth: {
    isEnabled: true,
    baseURL: "http://localhost:5003/",
    provider: {
      type: "local",
      sessionDataType: {
        id: "number",
        userName: "string",
        firstName: "string",
        lastName: "string",
        type: "number",
        email: "string",
        patients: "PatientDto[]",
      },
      endpoints: {
        signIn: {
          path: "/user/login",
        },
        signOut: {
          path: "/user/logout",
        },
        getSession: {
          path: "/user/session",
        },
      },
    },
    globalAppMiddleware: {
      isEnabled: true,
      allow404WithoutAuth: true,
    },
  },
  imports: {
    dirs: ["./types"],
  },
});
