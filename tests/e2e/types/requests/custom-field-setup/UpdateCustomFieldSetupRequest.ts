export default interface UpdateCustomFieldSetupRequest {
  id: string,
  name: string,
  description: string,
  type: string,
  dateCustomFieldSetup?: DateCustomFieldSetup,
  dateTimeCustomFieldSetup?: DateTimeCustomFieldSetup,
  durationCustomFieldSetup?: DurationCustomFieldSetup,
  multiSelectCustomFieldSetup?: MultiSelectCustomFieldSetup,
  numberCustomFieldSetup?: NumberCustomFieldSetup,
  singleSelectCustomFieldSetup?: SingleSelectCustomFieldSetup,
  textCustomFieldSetup?: TextCustomFieldSetup,
  timeCustomFieldSetup?: TimeCustomFieldSetup
}

interface DateCustomFieldSetup {
  defaultDate?: string
}

interface DateTimeCustomFieldSetup {
  defaultDateTime?: string
}

interface DurationCustomFieldSetup {
  defaultDuration?: string
}

interface MultiSelectCustomFieldSetup {
  operation?: string,
  newOption?: MultiSelectOption,
  optionId?: string,
  overOptionId?: string,
}

interface NumberCustomFieldSetup {
  settings: NumberSettings,
  defaultNumber?: number
}

interface SingleSelectCustomFieldSetup {
  defaultOptionId?: string,
  operation?: string,
  newOption?: SingleSelectOption,
  optionId?: string,
  overOptionId?: string,
}

interface TextCustomFieldSetup {
  defaultText?: string
}

interface TimeCustomFieldSetup {
  defaultTime?: string
}

interface MultiSelectOption {
  value: string,
  color: string
}

interface NumberSettings {
  currencyCode?: string,
  decimals: number,
  rounding: boolean,
}

interface SingleSelectOption {
  value: string,
  color: string
}