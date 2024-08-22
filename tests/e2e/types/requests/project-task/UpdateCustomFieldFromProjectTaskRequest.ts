export default interface UpdateCustomFieldFromProjectTaskRequest {
  id: string,
  customFieldId: string,
  type: string,
  numberCustomField?: NumberCustomField,
  singleSelectCustomField?: SingleSelectCustomField,
  textCustomField?: TextCustomField,
}

interface NumberCustomField {
  number?: number
}

interface SingleSelectCustomField {
  optionId?: string,
}

interface TextCustomField {
  text?: string
}