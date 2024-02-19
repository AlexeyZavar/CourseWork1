export function getPluralForm(forms: String[], n: number) {
  var ending;
  if (n % 10 === 1 && n % 100 !== 11) {
    ending = 0;
  } else if (n % 10 >= 2 && n % 10 <= 4 && (n % 100 < 10 || n % 100 >= 20)) {
    ending = 1;
  } else {
    ending = 2;
  }
  return n + " " + forms[ending] || "";
}

export function getPluralAge(n: number) {
  return getPluralForm(["год", "года", "лет"], n);
}

export const formatDateTime = (value: string) => {
  const date = new Date(value);

  if (date.getFullYear() == 1970) {
    return "∞";
  }

  return date.toLocaleString("ru-RU");
};
