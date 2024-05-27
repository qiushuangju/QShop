import { NUMBER } from '@/components/cron/constant/reg'

export const sortNum = (a, b) => {
  return a - b
}
export const isNumber = (str) => {
  return new RegExp(NUMBER).test(str)
}
export const getLocale = () => {
  return (localStorage.getItem('locale') ||
      sessionStorage.getItem('locale') ||
      (navigator.systemLanguage ? navigator.systemLanguage : navigator.language)).replace('-', '_')
}
