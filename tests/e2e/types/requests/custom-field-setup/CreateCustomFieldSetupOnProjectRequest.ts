export default interface CreateCustomFieldSetupOnProjectRequest {
  projectId: string,
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
  options: SingleSelectOption[],
  defaultOptionId?: string
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
  id: string,
  value: string,
  color: string
}