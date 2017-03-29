import React from 'react';

import Welcome from '../src/welcome.component';
import renderer from 'react-test-renderer';

it('renders correctly', () => {
    const tree = renderer.create(
        <Welcome name="TestName" />
    ).toJSON();
    expect(tree).toMatchSnapshot();
});