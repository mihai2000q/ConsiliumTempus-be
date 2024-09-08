export default interface CreateCustomFieldSetupOnProjectRequest {
  projectId: string,
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
  options: MultiSelectOption[]
}

interface NumberCustomFieldSetup {
  settings: NumberSettings,
  defaultNumber?: number
}

interface SingleSelectCustomFieldSetup {
  options: SingleSelectOption[],
  defaultOptionId?: string
}

interface TextCustomFieldSetup {
  defaultText?: string
}

interface MultiSelectOption {
  value: string,
  color: string
}

interface TimeCustomFieldSetup {
  defaultTime?: string
}

interface NumberSettings {
  currencyCode?: string,
  decimals: number,
  rounding: boolean,
}

interface SingleSelectOption {
  id: string,
  value: string,
  color: string
}