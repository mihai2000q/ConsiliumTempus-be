import { expect as baseExpect } from "@playwright/test";

export const expect = baseExpect.extend({
  toBeString(received: any) {
    if (typeof received == 'string') {
      return {
        message: () => 'passed',
        pass: true,
      };
    } else {
      return {
        message: () =>
          `toBeString() assertion failed.\n${received} was expected to be a string but it is a ${typeof received}\n`,
        pass: false,
      }
    }
  }
})