export function useIsAdmin() {
  const { data: session } = useAuth();
  return computed(() => session.value?.type === 2);
}
