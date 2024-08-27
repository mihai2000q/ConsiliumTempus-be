export default interface UpdateCustomFieldSetupRequest {
  id: string,
  name: string,
  description: string,
  type: string,
  numberCustomFieldSetup?: NumberCustomFieldSetup,
  singleSelectCustomFieldSetup?: SingleSelectCustomFieldSetup,
  textCustomFieldSetup?: TextCustomFieldSetup
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

interface NumberSettings {
  currencyCode?: string,
  decimals: number,
  rounding: boolean,
}

interface SingleSelectOption {
  value: string,
  color: string
}