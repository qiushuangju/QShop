export const
  /** 12 months */
  JAN = 'JAN'
export const FEB = 'FEB'
export const MAR = 'MAR'
export const APR = 'APR'
export const MAY = 'MAY'
export const JUN = 'JUN' // 1 - 6
export const
  JUL = 'JUL'
export const AUG = 'AUG'
export const SEP = 'SEP'
export const OCT = 'OCT'
export const NOV = 'NOV'
export const DEC = 'DEC' // 7 - 12
export const
  MONTHS = [JAN, FEB, MAR, APR, MAY, JUN, JUL, AUG, SEP, OCT, NOV, DEC]
export const
  SUN = 'SUN'
export const MON = 'MON'
export const TUE = 'TUE'
export const WED = 'WED'
export const THU = 'THU'
export const FRI = 'FRI'
export const SAT = 'SAT' /** 7 days of week */
export const
  DAYS_OF_WEEK = [SUN, MON, TUE, WED, THU, FRI, SAT]
export const
  /** symbols */
  EVERY = '*'
export const
  PERIOD = '/'
export const
  RANGE = '-'
export const
  FIXED = ','
export const
  UNFIXED = '?'
export const
  LAST = 'L'
export const
  WORK_DAY = 'W'
export const
  WEEK_DAY = '#'
export const
  CALENDAR = 'C'
export const
  BASE_SYMBOL = EVERY + ' ' + PERIOD + ' ' + RANGE + ' ' + FIXED
export const
  DAY_OF_MONTH_SYMBOL = BASE_SYMBOL + ' ' + LAST + ' ' + WORK_DAY + ' ' + CALENDAR
export const
  DAY_OF_WEEK_SYMBOL = BASE_SYMBOL + ' ' + UNFIXED + ' ' + LAST + ' ' + WEEK_DAY + ' ' + CALENDAR
export const
  EMPTY = ''
export const
  LAST_WORK_DAY = 'LW'
export const
  // current year like 2019
  CUR_YEAR = new Date().getFullYear()
  //
export const
  UPPER_LIMIT_YEAR = 2099
export const
  // default cron expression
  DEFAULT_CRON_EXPRESSION = '0 * * * * ?'
