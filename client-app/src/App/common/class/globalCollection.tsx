export default function convertStringToAscii(value: string) {
  let u = "";
  for (var i = 0; i < value.length; i++) {
    u += value.charCodeAt(i) + "-";
  }
  return u;
}
export function convertAsciiToString(value: string) {
  let valuefromsplit = value.split("-");
  let returnedValue = "";
  valuefromsplit.forEach((value, index) => {
    returnedValue += String.fromCharCode(Number(value));
  });
  return returnedValue;
}
export function sleep(ms: number) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}
