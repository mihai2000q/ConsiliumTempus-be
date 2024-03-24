export {};

declare global {
  namespace PlaywrightTest {
    interface Matchers<R> {
      toBeString(): R;
    }
  }
}