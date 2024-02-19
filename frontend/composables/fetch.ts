export function apiFetch<T>(
  request: Parameters<typeof useFetch<T>>[0],
  opts?: Parameters<typeof useFetch<T>>[1],
): ReturnType<typeof useFetch<T>> {
  const { token } = useAuth();
  const config = useRuntimeConfig();

  const interceptedRequest =
    (typeof request === "string" || request instanceof String) &&
    !request.startsWith("http")
      ? `${config.public.apiURL}${request}`
      : request;

  return useFetch<T>(interceptedRequest, {
    ...opts,
    headers: {
      Authorization: `${token.value}`,
      ...opts?.headers,
    },
  });
}
