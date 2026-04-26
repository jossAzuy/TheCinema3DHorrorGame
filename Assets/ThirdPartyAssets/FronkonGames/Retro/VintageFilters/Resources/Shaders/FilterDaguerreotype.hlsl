////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Martin Bustos @FronkonGames <fronkongames@gmail.com>. All rights reserved.
//
// THIS FILE CAN NOT BE HOSTED IN PUBLIC REPOSITORIES.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma once

float _DaguerreotypeContrast;
float _DaguerreotypeSilvering;

inline float3 FilterDaguerreotype(float3 pixel, float2 uv)
{
  float gray = dot(pixel, float3(0.299, 0.587, 0.114));
  
  // High contrast.
  float contrast = (gray - 0.5) * (1.0 + _DaguerreotypeContrast * 2.0) + 0.5;
  
  // Metallic silver tint.
  float3 silver = float3(contrast, contrast * 0.95, contrast * 0.9);
  
  // Add some "metallic" reflection simulation (very basic).
  float reflection = sin(uv.x * 10.0 + uv.y * 5.0) * 0.05 * _DaguerreotypeSilvering;
  silver += reflection;

  return clamp(silver, 0.0, 1.0);
}
