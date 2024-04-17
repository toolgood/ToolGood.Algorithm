QUnit.module('test-string', function () {
    QUnit.test('Test', async function (assert) {
        await new Promise(resolve => setTimeout(resolve, 300));
        assert.equal(1, 1);

    });
});